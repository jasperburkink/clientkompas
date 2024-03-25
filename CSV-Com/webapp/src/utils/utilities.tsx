export const isNullOrEmpty = (value: string | null | undefined): boolean => {
  return !value || value === undefined || value === '' || value.length === 0;
};

export const numberToBoolean = (number: number) =>{
    return Boolean(number);
}

export const objectToBoolean = (object: any) =>{
    return Boolean(object);
}

export const booleanToNumber = (boolean: boolean) =>{
    return Number(boolean);
}

