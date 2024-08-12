import { ValidationErrorHash } from "./validation-error";

export default interface ApiResult <T> {
    Ok: boolean;
    Title?: string;    
    Errors?: string[];
    ValidationErrors?: ValidationErrorHash;
    ReturnObject?: T;
}