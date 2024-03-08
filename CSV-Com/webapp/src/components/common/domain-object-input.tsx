import { useState }  from 'react';
import { Label } from '../../components/common/label';
import { Button } from '../../components/common/button';
import LabelField from '../../components/common/label-field';
import { InputField } from '../../components/common/input-field';
import { DatePicker } from '../../components/common/datepicker';
import './domain-object-input.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark} from "@fortawesome/free-solid-svg-icons";
import CustomLabels from '../../types/common/labels';

interface DomainObjectInputProps<T>{
    label: string;
    labelType: string;
    domainObjects: T[];
    numMinimalRequired?: number;    
    addObject: () => T;
    typeName: keyof typeof CustomLabels;
}

const DomainObjectInput = <T extends Record<string, any>>(props: DomainObjectInputProps<T>) => {
        const [domainObjects, setDomainObjects] = useState<T[]>(() => {
            const defaultObjects:T[] = props.domainObjects;

            if(props.numMinimalRequired) {
                for (let i = 0; defaultObjects.length < props.numMinimalRequired && i < props.numMinimalRequired; i++) {
                    const newObject: T = props.addObject();
                    defaultObjects.push(newObject);
                }
            }

            return defaultObjects;
        });

        const handleAddObject = () => {
            const newDomainObject: T = props.addObject();
            setDomainObjects([...domainObjects, newDomainObject]);
        };

        const handleRemoveObject = (domainObjectToRemove: T) => {
            const updatedDomainObjects: T[] = domainObjects.filter(obj => obj !== domainObjectToRemove);
            setDomainObjects(updatedDomainObjects);
        };

        // if(numMinimalRequired && domainObjects.length <= numMinimalRequired)

        const inputFields = domainObjects.map((domainObject, index) => {            
            const customLabelsForInterface = CustomLabels[props.typeName] as { [key: string]: string };
            let requiredDomainObject: boolean = props.numMinimalRequired !== null && index < props.numMinimalRequired!;
            const objectEntries = Object.entries(domainObject);
            const isOddNumberOfFields = objectEntries.length % 2 !== 0;

            return (
                <div key={index} className='domain-object-container'>
                    <div className="domain-object-container-item">
                    {objectEntries.map(([key, value], idx) => {
                        let textValue: string  = customLabelsForInterface[key] ? customLabelsForInterface[key] : key;
                        let inputType: string = 'text';
                        inputType = getInputFieldType(value, inputType);

                        const isFirstField = idx === 0;
                        const isLastField = idx === objectEntries.length - 1;

                        return (     
                            getDomainObjectField<T>(key, inputType, textValue, value, requiredDomainObject, isLastField, handleRemoveObject, domainObject, isOddNumberOfFields, isFirstField)
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
        <Button buttonType={{type:"Underline"}} text={`Voeg nog een ${props.labelType} toe`} className='domain-object-add-button' onClick={handleAddObject} />
        </>;
};

export default DomainObjectInput;

function getDomainObjectField<T extends Record<string, any>>(key: string, inputType: string, textValue: string, value: any, requiredDomainObject: boolean, isLastField: boolean, handleRemoveObject: (domainObjectToRemove: T) => void, domainObject: T, isOddNumberOfFields: boolean, isFirstField: boolean) {
    return <>
        <div key={key} className="domain-object-field">
            {inputType === 'date' ? getDateField(textValue, requiredDomainObject, value) : getTextField(textValue, requiredDomainObject, value)}
            {isLastField && !requiredDomainObject && (
                <div className='domain-object-remove-button' onClick={() => { handleRemoveObject(domainObject); } }>
                    <FontAwesomeIcon icon={faXmark} className="fa-solid fa-xl" />
                </div>
            )}
        </div>
        {isOddNumberOfFields && isFirstField && <div></div>}
    </>;
}

function getDateField(textValue: string, requiredDomainObject: boolean, value: Date) {
    return <div>
        <LabelField text={textValue} required={requiredDomainObject}>
            <DatePicker placeholder='Selecteer een datum' required={requiredDomainObject} value={value} />
        </LabelField>        
    </div>;
}

function getTextField(textValue: string, requiredDomainObject: boolean, value: string) {
    return <div className="flex-grow">
        <LabelField text={textValue} required={requiredDomainObject}>
            <InputField inputfieldtype={{type:'text'}} value={value} required={requiredDomainObject} placeholder='TODO:placeholder' />
        </LabelField>
    </div>;
}

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
