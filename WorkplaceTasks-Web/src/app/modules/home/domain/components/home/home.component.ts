import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { TaskResponseDto, TaskStatus } from '../../../infra/dto/task-dto';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { HomeControllerService } from '../../controllers/home-controller.service';
import { PermissionsService } from 'src/app/core/services/permissions.service';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, AfterViewInit {
    displayedColumns: string[] = ['title', 'description', 'status', 'createdAt', 'actions'];
    tasks = new MatTableDataSource<TaskResponseDto>([]);
    allTasks: TaskResponseDto[] = [];
    isLoading = false;
    userName: string = '';
    userRole: string = '';
    selectedStatus: TaskStatus | null = null;
    TaskStatus = TaskStatus; // Expor enum para o template
    
    @ViewChild(MatPaginator) paginator!: MatPaginator;

    constructor(
        private dialog: MatDialog,
        private snackBar: MatSnackBar,
        private router: Router,
        private controller: HomeControllerService,
        public permissions: PermissionsService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.loadUserInfo();
        this.loadTasks();
    }

    loadUserInfo(): void {
        const user = this.authService.getCurrentUser();
        if (user) {
            // Suporta tanto UserName quanto userName (minúsculo)
            this.userName = (user as any).UserName || (user as any).userName || '';
            // Suporta tanto UserRole quanto userRole (minúsculo)
            this.userRole = (user as any).UserRole || (user as any).userRole || '';
        }
    }

    ngAfterViewInit(): void {
        this.tasks.paginator = this.paginator;
    }

    loadTasks(): void {
        this.isLoading = true;
        this.tasks.data = [];
        this.controller.listarTarefas(this);
    }

    onTasksLoaded(): void {
        // Garantir que o paginator seja configurado após os dados serem carregados
        if (this.paginator) {
            this.tasks.paginator = this.paginator;
        }
    }

    applyStatusFilter(): void {
        if (this.selectedStatus === null) {
            // Mostrar todas as tarefas
            this.tasks.data = this.allTasks;
        } else {
            // Filtrar por status
            this.tasks.data = this.allTasks.filter(task => task.status === this.selectedStatus);
        }
        
        // Resetar paginação para a primeira página
        if (this.paginator) {
            this.paginator.firstPage();
        }
    }

    openAddDialog(): void {
        this.controller.adicionarTarefa(this);
    }

    editTask(task: TaskResponseDto): void {
        this.controller.editarTarefa(this, task);
    }

    deleteTask(task: TaskResponseDto): void {
        this.controller.excluirTarefa(this, task);
    }

    logout(): void {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        this.router.navigate(['/login']);
        this.snackBar.open('Logout realizado com sucesso', 'Fechar', {
            duration: 3000,
            panelClass: 'success-snackbar'
        });
    }

    getStatusLabel(status: TaskStatus): string {
        const statusMap: Record<TaskStatus, string> = {
            [TaskStatus.Pending]: 'Pendente',
            [TaskStatus.InProgress]: 'Em Progresso',
            [TaskStatus.Done]: 'Concluída',
            [TaskStatus.Deleted]: 'Excluída'
        };
        return statusMap[status] || 'Desconhecido';
    }

    getStatusClass(status: TaskStatus): string {
        const classMap: Record<TaskStatus, string> = {
            [TaskStatus.Pending]: 'status-pending',
            [TaskStatus.InProgress]: 'status-in-progress',
            [TaskStatus.Done]: 'status-completed',
            [TaskStatus.Deleted]: 'status-deleted'
        };
        return classMap[status] || '';
    }

    formatDate(dateString: string): string {
        const date = new Date(dateString);
        return date.toLocaleDateString('pt-BR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    }
}
