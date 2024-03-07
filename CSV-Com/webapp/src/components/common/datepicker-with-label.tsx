import React from 'react';
import './datepicker-with-label.css';
import { DatePicker, DatePickerProps } from '../../components/common/datepicker';

interface DatePickerWithLabelProps{
    text: string,
    datePickerProps: DatePickerProps,
    className?: string
}

export const DatePickerWithLabel = (props: DatePickerWithLabelProps) => (
    <div className={'datepicker-with-label ' + props.className}>
        <label htmlFor={props.text}>{props.text}{props.datePickerProps.required ? '*' : ''}</label>
        <DatePicker 
            placeholder={props.datePickerProps.placeholder} 
            required={props.datePickerProps.required}
            className={props.datePickerProps.className} 
            value={props.datePickerProps.value} />
    </div>
);