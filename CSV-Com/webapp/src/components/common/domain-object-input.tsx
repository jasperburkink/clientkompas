import { useState }  from 'react';
import { Label } from '../../components/common/label';
import { Button } from '../../components/common/button';
import { DatePickerWithLabel } from '../../components/common/datepicker-with-label';
import { InputFieldWithLabel } from '../../components/common/input-field-with-label';
import './domain-object-input.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark} from "@fortawesome/free-solid-svg-icons";

interface DomainObjectInputProps<T>{
    label: string;
    domainObjects: T[];
    // emptyObject: T;
    numMinimalRequired?: number;
    addObject: void;
    removeObject: void;
}

const DomainObjectInput = <T extends Record<string, any>>(props: DomainObjectInputProps<T>) => {    

      const [domainObjects, setDomainObjects] = useState<T[]>(props.domainObjects);

      const inputFields = domainObjects.map((domainObject, index) => {
        return (
            <>
            <div key={index} className='domain-object-container'>
                {Object.entries(domainObject).map(([key, value]) => {
                    let inputType: string = 'text';
                    inputType = getInputFieldType(value, inputType);

                    return (                        
                        <div key={key}>
                            {inputType === 'date' ? (
                                <div>
                                    <DatePickerWithLabel 
                                        text={key}
                                        datePickerProps={{                         
                                            value: value as Date,
                                            placeholder:'Selecteer een datum'//TODO: required
                                        }}
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
                {/* <div className='p-3 border-2 rounded-lg float-right w-12 h-12 mt-4 border-subGray2 bg-mainLightGray cursor-pointer' onClick={() => props.removeObject}>
                    <FontAwesomeIcon icon={faXmark} className="domain-object-remove-button flex-none fa-solid fa-xl"  />
                </div> */}
            </div>
            </>
        );
    });

    return <>
        <Label text={props.label} className='domain-object-label' strong={true} />
        {inputFields}
        {/* <Button buttonType={{type:"Underline"}} text={`Voeg nog een ${typeof props.domainObjects} toe`} className='domain-object-add-button' onClick={()=> {props.addObject}} /> */}
        </>;
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
