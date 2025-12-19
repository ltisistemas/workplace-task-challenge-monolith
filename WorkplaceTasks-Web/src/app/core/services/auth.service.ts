import { Injectable } from '@angular/core';
import { LoginResponseDto } from 'src/app/modules/auth/infra/dto/login-dto';

export enum UserRole {
    Admin = 'Admin',
    Manager = 'Manager',
    Member = 'Member'
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    getCurrentUser(): LoginResponseDto | null {
        const userStr = localStorage.getItem('user');
        if (!userStr) return null;
        try {
            return JSON.parse(userStr) as LoginResponseDto;
        } catch {
            return null;
        }
    }

    getCurrentUserId(): string | null {
        const user = this.getCurrentUser();
        if (!user) return null;
        // Suporta tanto Id quanto id (minúsculo)
        return (user as any).Id || (user as any).id || null;
    }

    getCurrentUserRole(): UserRole | null {
        const user = this.getCurrentUser();
        if (!user) return null;
        // Suporta tanto UserRole quanto userRole (minúsculo)
        const role = (user as any).UserRole || (user as any).userRole;
        if (!role) return null;
        return role as UserRole;
    }

    isAdmin(): boolean {
        return this.getCurrentUserRole() === UserRole.Admin;
    }

    isManager(): boolean {
        return this.getCurrentUserRole() === UserRole.Manager;
    }

    isMember(): boolean {
        return this.getCurrentUserRole() === UserRole.Member;
    }

    isOwner(taskUserId: string): boolean {
        const currentUserId = this.getCurrentUserId();
        return currentUserId === taskUserId;
    }
}

