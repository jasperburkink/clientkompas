import React from 'react';
import { DatePicker } from '../../components/common/datepicker';
import { InputFieldWithLabel } from '../../components/common/input-field-with-label';

interface DomainObjectInputProps<T>{
    domainObjects: T[];
    onChange: (fieldName: keyof T, value: string | Date, index: number) => void;
}

const DomainObjectInput = <T extends Record<string, any>>(props: DomainObjectInputProps<T>) => {
    const handleChange = (fieldName: keyof T, value: string | Date, index: number) => {
        props.onChange(fieldName, value, index);
    };

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
                                        onChange={(date: Date | null) => handleChange(key as keyof T, date || new Date(), index)}
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
                                            value: value as string,
                                            onChange: (newValue: string) =>
                                                handleChange(key as keyof T, newValue, index),
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
