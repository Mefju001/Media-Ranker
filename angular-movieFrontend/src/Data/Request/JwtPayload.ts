export interface JwtPayload {
    name?: string;
    unique_name?: string;
    role?: string | string[];
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'?: string;
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
}