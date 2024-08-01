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
import { ValidationError, ValidationErrorHash } from 'types/common/validation-error';
import WorkingContract from 'types/model/WorkingContract';
import RequiredFields, { RequiredFieldsConfig } from 'types/common/required-fields';

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
    dataTestId?: string;
    errors?: ValidationErrorHash;
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
                        let dataTestId: string = `${props.dataTestId}.${key}.${(domainIndex+1)}`;
                        let error: string | undefined = props.errors?.[`${props.typeName}[${domainIndex}].${key.toLowerCase()}`]?.errormessage;                        
                        let requiredField: boolean = isFieldRequired(props.typeName, key);

                        console.log(`${key}:${requiredField}`);

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
                                requiredField,
                                isLastField, 
                                handleRemoveObject, 
                                domainObject, 
                                isOddNumberOfFields, 
                                isFirstField,
                                onChangeField,
                                domainIndex,
                                dataTestId,
                                options,
                                error
                            )
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
        <Button buttonType={{type:"Underline"}} text={`Voeg nog een ${props.labelType} toe`} className='domain-object-add-button' dataTestId={`${props.dataTestId}.add`} onClick={handleAddObject} />
        </>;
};

export default DomainObjectInput;

function getDomainObjectField<T extends Record<string, any>>(
    key: string, 
    inputType: string, 
    textValue: string, 
    value: any, 
    requiredDomainObject: boolean, 
    requiredField: boolean,
    isLastField: boolean, 
    handleRemoveObject: (domainObjectToRemove: T) => void, 
    domainObject: T, 
    isOddNumberOfFields: boolean, 
    isFirstField: boolean, 
    onChange: (updatedValue: any, index: number, inputType: string) => void, 
    domainIndex: number,
    dataTestId: string,
    options?: DropdownObject[],
    error?: string) {
    let fieldComponent;
    
    switch (inputType) {
        case 'date':
          fieldComponent = getDateField(textValue, requiredField, value, onChange, inputType, domainIndex, dataTestId, error);  
            break;
        case 'dropdown':
            options 
            ? fieldComponent = getDropdownField(textValue, requiredField, value, onChange, inputType, domainIndex, options, dataTestId, error)
            : fieldComponent = getTextField(textValue, requiredField, value, onChange, inputType, domainIndex, dataTestId, error);
            break;
        default:
            fieldComponent = getTextField(textValue, requiredField, value, onChange, inputType, domainIndex, dataTestId, error);
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
    requiredField: boolean,
    value: Date,
    onChange: (updatedValue: Moment | null, index: number, inputType: string) => void,
    inputType: string,
    index: number,
    dataTestId: string,
    error?: string) {
    return <div className="domain-object-field-container">
        <LabelField text={textValue} required={requiredField}>
            <DatePicker 
                placeholder='b.v. 01-01-2001' 
                required={requiredField} 
                value={value}
                onChange={(newValue) => {onChange(newValue, index, inputType)}}
                dataTestId={dataTestId}
                error={error} />
        </LabelField>   
    </div>;
}

function getTextField(
    textValue: string, 
    requiredField: boolean,
    value: string, 
    onChange: (updatedValue: string, index: number, inputType: string) => void, 
    inputType: string,
    domainIndex: number,
    dataTestId: string,
    error?: string) {
    return <div className="domain-object-field-container">
        <LabelField text={textValue} required={requiredField}>
            <InputField 
                inputfieldtype={{type:'text'}}                
                value={value}
                required={requiredField} 
                placeholder={textValue} 
                onChange={(newValue) => {onChange(newValue, domainIndex, inputType)}}
                dataTestId={dataTestId}
                error={error} />
        </LabelField>
    </div>;
}

function getDropdownField(
    textValue: string, 
    requiredField: boolean,
    value: number, 
    onChange: (updatedValue: string | number, index: number, inputType: string) => void, 
    inputType: string,    
    index: number,
    options: DropdownObject[],
    dataTestId: string,
    error?: string) {
    return <div className="domain-object-field-container">
        <LabelField text={textValue} required={requiredField}>
            <Dropdown 
                inputfieldname={textValue}
                options={options}
                value={value}
                required={requiredField}
                onChange={(newValue) => {onChange(newValue, index, inputType)}}
                dataTestId={dataTestId}
                error={error} />
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

function isFieldRequired<T>(typeName: keyof RequiredFieldsConfig, fieldName: keyof T): boolean {
    return RequiredFields[typeName]?.[fieldName] ?? false; // Standaard op false als het type of veld niet gevonden wordt
}