import Decimal from "decimal.js-light";

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

export const formatCurrency = (amount: Decimal) => {
    return amount
        .toFixed(2) // Rondt af op twee decimalen
        .replace('.', ',') // Vervangt de punt met een komma voor de decimalen
        .replace(/\B(?=(\d{3})+(?!\d))/g, "."); // Voeg een punt toe voor duizendtallen
};
