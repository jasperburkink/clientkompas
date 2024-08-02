import React, { useState, useEffect } from 'react';
import './datepicker.css';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import * as DatePickerControl from '@mui/x-date-pickers/DatePicker';
import * as MobileDatePickerControl from '@mui/x-date-pickers/MobileDatePicker';
import 'moment/locale/nl';
import Moment from 'moment';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from "@fortawesome/free-solid-svg-icons";
import styled from '@emotion/styled';
import { DateTimePicker } from '@mui/x-date-pickers';
import { ErrorMessage } from './error-message';
import { ValidationError } from 'types/common/validation-error';

const LOCALE_PROVIDER = "nl";
const MOBILE_BREAKPOINT = 640;

export interface DatePickerProps {
    placeholder: string,
    value?: Date | null,
    required: boolean,
    className?: string,
    onChange?: (value: Moment.Moment | null) => void,
    dataTestId?: string,
    errors?: ValidationError[];
}

function GenerateAwesomeFontCalendarIcon() {
  return <FontAwesomeIcon icon={faCalendar} className="fa my-auto cursor-pointer" />;
}

const handleResize = (setIsMobileView: React.Dispatch<React.SetStateAction<boolean>>) => {
    setIsMobileView(window.innerWidth <= MOBILE_BREAKPOINT);
};

const DATE_FORMAT = 'DD-MM-yyyy';

export const DatePicker = (props: DatePickerProps) => {
  const [isMobileView, setIsMobileView] = useState<boolean>(false);

  // Show dates in local format
  Moment.locale('nl');

  useEffect(() => {
    const handleResizeCallback = () => handleResize(setIsMobileView);

    // Eventlistener when changing screen size
      window.addEventListener('resize', handleResizeCallback);

    // Initialize when loading the page
      handleResizeCallback();

    // Remove eventlistener when component is disposed
    return () => {
        window.removeEventListener('resize', handleResizeCallback);
    };
  }, []);

  const handleDateChange = (date: Moment.Moment | null) => {
    if (date !== null) {
      props.onChange?.(Moment(date));
    } else {
      props.onChange?.(null);
    }
  };

  return (
    <div className='datepicker-container'>
      <LocalizationProvider dateAdapter={AdapterMoment} adapterLocale={LOCALE_PROVIDER}>
        {isMobileView ? (
          <MobileDatePickerControl.MobileDatePicker
          className={`datepicker ${props.className}`}      
          value={props.value ? Moment(props.value) : null}
          onChange={handleDateChange}
          slotProps={{
            textField: {
              inputProps: {
                'data-testid': props.dataTestId
              }
            }
          }} />
        ): (
          <DatePickerControl.DatePicker      
            className={`datepicker ${props.className}`}
            value={props.value ? Moment(props.value) : null}
            slots={{openPickerIcon: GenerateAwesomeFontCalendarIcon}}
            slotProps={{textField: {
              required: props.required,
              inputProps: {
                  'data-testid': props.dataTestId
              }
            }}}        
            onChange={handleDateChange}
            sx={{
              '& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline': { border: '2px solid b3b3b3' }, //Init state
              '& .MuiOutlinedInput-root:hover .MuiOutlinedInput-notchedOutline': { border: '2px solid #b3b3b3' },  // at hover state
              '& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline': { border: '2px solid #148CB8' }, // at focused state
          }}
            />
        )}
      </LocalizationProvider>  
      <ErrorMessage errors={props.errors} />
  </div>
  );
};