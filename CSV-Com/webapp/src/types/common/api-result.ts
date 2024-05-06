export default interface ApiResult <T> {
    Ok: boolean;
    Errors?: string[];
    ReturnObject?: T;
}