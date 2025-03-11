import { ValidationErrorHash } from "./validation-error";

export default interface ApiResult <T> {
    succeeded: boolean;
    value?: T;
    errors?: string[];
    errormessage?: string;
    validationerrors?: ValidationErrorHash;
}