import { useEffect, useState } from "react";
import { Button, ButtonProps } from "./button";
import moment from "moment";

export interface CountdownButtonProps extends ButtonProps {
    countdownMax: number;
}

export const CountdownButton = (props: CountdownButtonProps) => {

    const [timeoutSeconds, setTimeoutSeconds] = useState<number>(0);

    useEffect(() => {
        if (timeoutSeconds > 0) {
            const interval = setInterval(() => {
                setTimeoutSeconds((prev) => Math.max(prev - 1, 0));
            }, 1000);

            return () => clearInterval(interval);
        }
      }, [timeoutSeconds]);

    const getTimeoutText = (remainingSeconds: number): string => {
        if(remainingSeconds <= 0){
            return props.text;
        }

        let duration = moment.duration(remainingSeconds, 'seconds');
        return `${duration.asSeconds()} seconden`;
    };

    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        if(timeoutSeconds > 0){
            return;
        }

        setTimeoutSeconds(props.countdownMax);
                
        if (props.onClick) {
            props.onClick(event);
        }
    };

    return (
        <>
        <Button {...props} buttonType={props.buttonType} onClick={handleClick} text={getTimeoutText(timeoutSeconds)} />
        </>
    )
}
