export interface LoginRequestDto {
    email: string;
    password: string;
}

export interface LoginResponseDto {
    token: string;
    Id: string;
    UserName: string;
    UserEmail: string;
    UserRole: string;
}