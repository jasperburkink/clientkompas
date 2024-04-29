import { useState }  from 'react';
import { Label } from 'components/common/label';
import { Button } from 'components/common/button';
import LabelField from 'components/common/label-field';
import { InputField } from 'components/common/input-field';
import { DatePicker } from 'components/common/datepicker';
import './domain-object-input.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark} from "@fortawesome/free-solid-svg-icons";
import CustomLabels from 'types/common/labels';
import moment, { Moment } from 'moment';
import { Dropdown, DropdownObject } from './dropdown';

interface DomainObjectInputProps<T>{
    className?: string;
    label: string;
    labelType: string;
    value: T[];
    fieldOrder?: string[];
    numMinimalRequired: number;    
    addObject: () => T;
    typeName: keyof typeof CustomLabels;
    onRemoveObject: (removedObject: T) => void;
    onChangeObject: (updatedObject: T, index: number) => void,
    optionsDictionary?: { [key: string] : DropdownObject[] };
}

const DomainObjectInput = <T extends Record<string, any>>(props: DomainObjectInputProps<T>) => {
        const [domainObjects, setDomainObjects] = useState<T[]>(() => {
            const defaultObjects:T[] = props.value;

            for (let i = 0; defaultObjects.length < Math.max(props.numMinimalRequired, 1); i++) {
                const newObject: T = props.addObject();
                defaultObjects.push(newObject);
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
            props.onRemoveObject(domainObjectToRemove);
        };

        const inputFields = domainObjects.map((domainObject, domainIndex) => {            
            const orderedFields = props.fieldOrder || Object.keys(domainObject);
            const customLabelsForInterface = CustomLabels[props.typeName] as { [key: string]: string };
            let requiredDomainObject: boolean = props.numMinimalRequired !== null && domainIndex < props.numMinimalRequired!;
            const isOddNumberOfFields = orderedFields.length % 2 !== 0;

            return (
                <div key={domainIndex} className={`domain-object-container`}>
                    <div className="domain-object-container-item">
                    {orderedFields.map((key, fieldIndex) => {
                        let value = domainObject[key];
                        let textValue: string  = customLabelsForInterface[key] ? customLabelsForInterface[key] : key;
                        let inputType: string = 'text';
                        inputType = getInputFieldType(value, key, props.optionsDictionary);

                        const isFirstField = fieldIndex === 0;
                        const isLastField = fieldIndex === orderedFields.length - 1;

                        const onChangeField = (updatedValue: any, index: number, inputType: string) => {
                            const updatedDomainObjects = [...domainObjects];

                            if(inputType === 'date') {
                                updatedValue = moment(updatedValue).toDate();
                            }

                            const updatedDomainObject = { ...domainObject, [key]: updatedValue };
                            updatedDomainObjects[index] = updatedDomainObject;
                            setDomainObjects(updatedDomainObjects);

                            props.onChangeObject(updatedDomainObject, index);
                        };

                        let options;
                        if(inputType === 'dropdown' && 
                        props.optionsDictionary &&
                        props.optionsDictionary[key]) {
                            options = props.optionsDictionary[key];
                        }
                        
                        return (     
                            getDomainObjectField<T>(
                                key, 
                                inputType, 
                                textValue, 
                                value, 
                                requiredDomainObject, 
                                isLastField, 
                                handleRemoveObject, 
                                domainObject, 
                                isOddNumberOfFields, 
                                isFirstField,
                                onChangeField,
                                domainIndex,
                                options)
                        );
                    })}
                    </div>              
                </div>
            );
        }
    );

    return <>
        <Label text={props.label} className={`domain-object-label ${props.className}`} strong={true} />
        {inputFields}
        <Button buttonType={{type:"Underline"}} text={`Voeg nog een ${props.labelType} toe`} className='domain-object-add-button' onClick={handleAddObject} />
        </>;
};

export default DomainObjectInput;

function getDomainObjectField<T extends Record<string, any>>(
    key: string, 
    inputType: string, 
    textValue: string, 
    value: any, 
    requiredDomainObject: boolean, 
    isLastField: boolean, 
    handleRemoveObject: (domainObjectToRemove: T) => void, 
    domainObject: T, 
    isOddNumberOfFields: boolean, 
    isFirstField: boolean, 
    onChange: (updatedValue: any, index: number, inputType: string) => void, 
    domainIndex: number,
    options?: DropdownObject[]) {
    let fieldComponent;
    
    switch (inputType) {
        case 'date':
          fieldComponent = getDateField(textValue, requiredDomainObject, value, onChange, inputType, domainIndex);  
            break;
        case 'dropdown':
            options 
            ? fieldComponent = getDropdownField(textValue, requiredDomainObject, value, onChange, inputType, domainIndex, options)
            : fieldComponent = getTextField(textValue, requiredDomainObject, value, onChange, inputType, domainIndex);
            break;
        default:
            fieldComponent = getTextField(textValue, requiredDomainObject, value, onChange, inputType, domainIndex);
            break;
    }

    let firstField:boolean = domainIndex === 0;

    return <>
        <div key={key} className="domain-object-field">
            {fieldComponent}            
            {isLastField && !requiredDomainObject && !firstField && (
                <div className='domain-object-remove-button' onClick={() => { handleRemoveObject(domainObject); } }>
                    <FontAwesomeIcon icon={faXmark} className="fa-solid fa-xl" />
                </div>
            )}
        </div>
        {isOddNumberOfFields && isFirstField && <div></div>}
    </>;
}

function getDateField(
    textValue: string, 
    requiredDomainObject: boolean, 
    value: Date,
    onChange: (updatedValue: Moment | null, index: number, inputType: string) => void,
    inputType: string,
    index: number) {
    return <div className="domain-object-field-container">
        <LabelField text={textValue} required={requiredDomainObject}>
            <DatePicker 
                placeholder='b.v. 01-01-2001' 
                required={requiredDomainObject} 
                value={value}
                onChange={(newValue) => {onChange(newValue, index, inputType)}} />
        </LabelField>   
    </div>;
}

function getTextField(
    textValue: string, 
    requiredDomainObject: boolean, 
    value: string, 
    onChange: (updatedValue: string, index: number, inputType: string) => void, 
    inputType: string,
    domainIndex: number) {
    return <div className="domain-object-field-container">
        <LabelField text={textValue} required={requiredDomainObject}>
            <InputField 
                inputfieldtype={{type:'text'}}                
                value={value}
                required={requiredDomainObject} 
                placeholder={textValue} 
                onChange={(newValue) => {onChange(newValue, domainIndex, inputType)}} />
        </LabelField>
    </div>;
}

function getDropdownField(
    textValue: string, 
    requiredDomainObject: boolean, 
    value: number, 
    onChange: (updatedValue: string | number, index: number, inputType: string) => void, 
    inputType: string,    
    index: number,
    options: DropdownObject[]) {
    return <div className="domain-object-field-container">
        <LabelField text={textValue} required={requiredDomainObject}>
            <Dropdown 
                inputfieldname={textValue}
                options={options}
                value={value}
                required={requiredDomainObject}
                onChange={(newValue) => {onChange(newValue, index, inputType)}} />
        </LabelField>
    </div>;
}

function getInputFieldType(value: any, key: string, optionsDictionary?:{ [key: string]: DropdownObject[] }) {
    var inputType;

    switch (typeof value) {
        case 'string':
            inputType = 'text';
            break;
        case 'number':
            inputType = 'text';

            if(optionsDictionary && optionsDictionary[key]){
                inputType = 'dropdown';                
            }

            break;
        default:
            if (value instanceof Date || key.includes('date')) {
                inputType = 'date';
                break;
            }

            inputType = 'text';
            break;
    }

    return inputType;
}
