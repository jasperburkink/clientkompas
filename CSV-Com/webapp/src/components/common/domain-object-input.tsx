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
    numMinimalRequired?: number;    
    addObject: () => T;
    typeName: string;
}

const DomainObjectInput = <T extends Record<string, any>>(props: DomainObjectInputProps<T>) => {
        const [domainObjects, setDomainObjects] = useState<T[]>(props.domainObjects);

        const handleAddObject = () => {
            const newDomainObject: T = props.addObject();
            setDomainObjects([...domainObjects, newDomainObject]);
        };

        const handleRemoveObject = (domainObjectToRemove: T) => {
            const updatedDomainObjects: T[] = domainObjects.filter(obj => obj !== domainObjectToRemove);
            setDomainObjects(updatedDomainObjects);
        };

        const inputFields = domainObjects.map((domainObject, index) => {
            let requiredDomainObject: boolean = props.numMinimalRequired !== null && index <= props.numMinimalRequired!;
            const objectEntries = Object.entries(domainObject);
            const isOddNumberOfFields = objectEntries.length % 2 !== 0;

            return (
                <div key={index} className='domain-object-container flex flex-col lg:col-span-2'>
                    <div className="grid grid-cols-1 lg:grid-cols-2">
                    {objectEntries.map(([key, value], idx) => {
                        let inputType: string = 'text';
                        inputType = getInputFieldType(value, inputType);

                        const isFirstField = idx === 0;
                        const isLastField = idx === objectEntries.length - 1;

                        return (     
                            <>                   
                            <div key={key} className="flex items-center">
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
                                    <div className="flex-grow">
                                        <InputFieldWithLabel
                                            text={key}
                                            inputFieldProps={{
                                                required: requiredDomainObject,
                                                placeholder: 'Placeholder',
                                                inputfieldtype: { type: 'text' },
                                                value: value as string
                                            }}
                                        />
                                    </div>                                
                                )}
                                {isLastField && (
                                    <div className='p-3 mr-2 border-2 rounded-lg w-12 h-12 border-subGray2 bg-mainLightGray cursor-pointer' onClick={() => {handleRemoveObject(domainObject);}}>
                                        <FontAwesomeIcon icon={faXmark} className="domain-object-remove-button flex-none fa-solid fa-xl"  />
                                    </div>
                                )}                                
                            </div>
                            {isOddNumberOfFields && isFirstField && <div></div>}
                            </>
                        );
                    })}
                    </div>              
                </div>
            );
        }
    );

    return <>
        <Label text={props.label} className='domain-object-label' strong={true} />
        {inputFields}
        <Button buttonType={{type:"Underline"}} text={`Voeg nog een ${props.typeName} toe`} className='domain-object-add-button' onClick={handleAddObject} />
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
