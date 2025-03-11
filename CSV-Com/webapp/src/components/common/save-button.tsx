import React, { useState } from 'react';
import Spinner from './spinner-button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import ApiResult from 'types/common/api-result';

const RESET_TIMEOUT = 4000;

interface SaveButtonProps<T> {
  buttonText: string;
  loadingText: string;
  successText: string;
  errorText: string;
  onSave: () => Promise<ApiResult<T>>;
  onResult: (result: ApiResult<T>) => void;
  dataTestId?: string;
  className?: string;
}

const SaveButton = <T,>({ buttonText, loadingText, successText, errorText, onSave, onResult, dataTestId, className }: SaveButtonProps<T>) => {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState(false);
  const [showText] = useState(true);

  const saveAction = async () => {
      setLoading(true);

      var result = await onSave();
      setLoading(false);
      setSuccess(result.succeeded);

      if (result.succeeded === false) {
        setError(true);
      }

      setTimeout(() => {
        setSuccess(false);
        setError(false);
      }, RESET_TIMEOUT)

      onResult(result);  
  };

  return (
    <div>
      <button
        onClick={saveAction}
        className={`${IsButtonVisible(error, success)} ${className}`}
        disabled={loading || error || success}
        data-testid={dataTestId}
      >
        {loading && <Spinner data-testid="spinner" />}
        {success && (
          <div>
            <FontAwesomeIcon className='icon' icon={faCheck} data-testid="icon" />
            <span className="text">{successText}</span>
          </div>
        )}

        {error && (
          <div>
          <FontAwesomeIcon className='icon' icon={faTimes}   />
          <span className="text"> {errorText}</span>
          </div>
        )}

        {ButtonTextShowcase(showText, loading, success, error) && (
          <span className="text">
            {ButtonTextLoader(loading, loadingText, success, successText, buttonText)} 
          </span>
        )}
      </button>
    </div>
  );
};

export default SaveButton;

function IsButtonVisible(error: boolean, success: boolean): string | undefined {
  return `before button ${error ? 'error' : success ? 'success' : 'default'}`;
}

function ButtonTextShowcase(showText: boolean, loading: boolean, success: boolean, error: boolean) {
  return showText && !loading && !success && !error;
}

function ButtonTextLoader(loading: boolean, loadingText: string, success: boolean, successText: string, buttonText: string): React.ReactNode {
  return loading ? loadingText : success ? successText : buttonText;
}
