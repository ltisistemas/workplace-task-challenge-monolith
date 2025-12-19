import { Injectable } from '@angular/core';
import { AuthService, UserRole } from './auth.service';
import { TaskResponseDto, TaskStatus } from 'src/app/modules/home/infra/dto/task-dto';

@Injectable({
    providedIn: 'root'
})
export class PermissionsService {
    constructor(private authService: AuthService) { }

    canEditTask(task: TaskResponseDto): boolean {
        // Tarefas concluídas não podem ser editadas, apenas visualizadas
        if (task.status === TaskStatus.Done) {
            return false;
        }

        const role = this.authService.getCurrentUserRole();
        const isOwner = this.authService.isOwner(task.userId);

        if (role === UserRole.Admin) {
            return true; // Admin pode editar todas (exceto concluídas)
        }

        if (role === UserRole.Manager) {
            return true; // Manager pode editar todas (exceto concluídas)
        }

        if (role === UserRole.Member) {
            return isOwner; // Member só pode editar as que criou (exceto concluídas)
        }

        return false;
    }

    canDeleteTask(task: TaskResponseDto): boolean {
        // Tarefas concluídas não podem ser excluídas
        if (task.status === TaskStatus.Done) {
            return false;
        }

        const role = this.authService.getCurrentUserRole();
        const isOwner = this.authService.isOwner(task.userId);

        if (role === UserRole.Admin) {
            return true; // Admin pode excluir todas (exceto concluídas)
        }

        if (role === UserRole.Manager) {
            return isOwner; // Manager só pode excluir se for criador (exceto concluídas)
        }

        if (role === UserRole.Member) {
            return isOwner; // Member só pode excluir se for criador (exceto concluídas)
        }

        return false;
    }
}

