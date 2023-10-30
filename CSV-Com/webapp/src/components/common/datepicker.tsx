import React from 'react';
import './datepicker.css';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import * as DatePickerControl from '@mui/x-date-pickers/DatePicker';
import 'moment/locale/nl';

interface DatePickerProps {
    text: string
}

export const DatePicker = (props: DatePickerProps) => (
  <LocalizationProvider dateAdapter={AdapterMoment} adapterLocale="nl">
    <DatePickerControl.DatePicker />
  </LocalizationProvider>
);