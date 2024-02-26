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
        <label htmlFor={props.text}>{props.text}</label>
        <DatePicker 
            placeholder={props.datePickerProps.placeholder} 
            className={props.datePickerProps.className} 
            value={props.datePickerProps.value} />
    </div>
);