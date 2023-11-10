import React, { useState, useEffect } from 'react';
import './datepicker.css';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import * as DatePickerControl from '@mui/x-date-pickers/DatePicker';
import * as MobileDatePickerControl from '@mui/x-date-pickers/MobileDatePicker';
import 'moment/locale/nl';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from "@fortawesome/free-solid-svg-icons";

const LOCALE_PROVIDER = "nl";
const MOBILE_BREAKPOINT = 640;

interface DatePickerProps {
    placeholder: string
}

function GenerateAwesomeFontCalendarIcon() {
  return <FontAwesomeIcon icon={faCalendar} className="fa my-auto cursor-pointer" />;
}

const handleResize = (setIsMobileView: React.Dispatch<React.SetStateAction<boolean>>) => {
    setIsMobileView(window.innerWidth <= MOBILE_BREAKPOINT);
};

export const DatePicker = (props: DatePickerProps) => {
  const [isMobileView, setIsMobileView] = useState<boolean>(false);

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

  return (
  <LocalizationProvider dateAdapter={AdapterMoment} adapterLocale={LOCALE_PROVIDER}>
    {isMobileView ? (
      <MobileDatePickerControl.MobileDatePicker label={props.placeholder} className='bg-mainLightGray datepicker' />
    ): (
      <DatePickerControl.DatePicker 
        label={props.placeholder} 
        className='bg-mainLightGray datepicker' 
        slots={{ openPickerIcon: GenerateAwesomeFontCalendarIcon }}
        sx={{      
          '& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline': { border: '2px solid b3b3b3' }, //Init state
          '& .MuiOutlinedInput-root:hover .MuiOutlinedInput-notchedOutline': { border: '2px solid #b3b3b3' },  // at hover state
          '& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline': { border: '2px solid #148CB8' }, // at focused state
      }}
         />
    )}
  </LocalizationProvider>
  );
};