import React from 'react';
import { DatePicker } from '../../components/common/datepicker';
import { InputFieldWithLabel } from '../../components/common/input-field-with-label';

interface DomainObjectInputProps<T>{
    domainObjects: T[];
}

const DomainObjectInput = <T extends Record<string, any>>(props: DomainObjectInputProps<T>) => {    

      const inputFields = props.domainObjects.map((domainObject, index) => {
        return (
            <div key={index} className='domain-object-container'>
                {Object.entries(domainObject).map(([key, value]) => {
                    let inputType: string = 'text';
                    inputType = getInputFieldType(value, inputType);

                    return (
                        <div key={key}>
                            {inputType === 'date' ? (
                                <div>
                                    <label htmlFor={key}>{key}</label>
                                    <DatePicker
                                        key={key}
                                        value={value}
                                        placeholder='Selecteer een datum'
                                    />
                                </div>
                            ) : (
                                <div>
                                    <InputFieldWithLabel
                                        text={key}
                                        inputFieldProps={{
                                            required: false,
                                            placeholder: 'Placeholder',
                                            inputfieldtype: { type: 'text' },
                                            value: value as string
                                        }}
                                    />
                                </div>
                            )}
                        </div>
                    );
                })}
            </div>
        );
    });

    return <>{inputFields}</>;
};

export default DomainObjectInput;

function getInputFieldType(value: any, inputType: string) {
    switch (typeof value) {
        case 'string':
            inputType = 'text';
            break;
        default:
            if (value instanceof Date) {
                inputType = 'date';
                break;
            }

            inputType = 'text';
            break;
    }
    return inputType;
}
