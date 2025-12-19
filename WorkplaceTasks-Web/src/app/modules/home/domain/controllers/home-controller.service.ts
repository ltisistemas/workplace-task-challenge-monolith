import { Injectable } from '@angular/core';
import { ListarTarefasUsecaseService } from '../usecases/listar-tarefas-usecase.service';
import { CriarTarefaUsecaseService } from '../usecases/criar-tarefa-usecase.service';
import { EditarTarefaUsecaseService } from '../usecases/editar-tarefa-usecase.service';
import { ExcluirTarefaUsecaseService } from '../usecases/excluir-tarefa-usecase.service';
import { HomeComponent } from '../components/home/home.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { NovaTarefaComponent } from '../components/nova-tarefa/nova-tarefa.component';
import { EditarTarefaComponent } from '../components/editar-tarefa/editar-tarefa.component';
import { ConfirmarExclusaoComponent } from '../components/confirmar-exclusao/confirmar-exclusao.component';
import { TaskResponseDto, DeleteTaskRequestDto } from '../../infra/dto/task-dto';

@Injectable({
    providedIn: 'root'
})
export class HomeControllerService {
    constructor(
        private listarUsecase: ListarTarefasUsecaseService,
        private criarUsecase: CriarTarefaUsecaseService,
        private editarUsecase: EditarTarefaUsecaseService,
        private excluirUsecase: ExcluirTarefaUsecaseService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog
    ) { }

    listarTarefas(component: HomeComponent): void {
        this.listarUsecase.execute().subscribe({
            next: (response) => {
                try {
                    component.allTasks = response; // Armazenar todas as tarefas
                    component.tasks.data = response; // Inicialmente mostrar todas
                    component.isLoading = false;
                    // Configurar paginator após carregar dados
                    setTimeout(() => {
                        component.onTasksLoaded();
                    }, 0);
                } catch (error: any) {
                    component.isLoading = false;
                    this.snackBar.open(error.message, 'Fechar', {
                        duration: 3000,
                        panelClass: 'error-snackbar'
                    });
                }
            },
            error: (error) => {
                component.isLoading = false;
                const errorMessage = error?.error?.message || error?.message || 'Erro ao carregar tarefas';
                this.snackBar.open(errorMessage, 'Fechar', {
                    duration: 3000,
                    panelClass: 'error-snackbar'
                });
            }
        });
    }

    adicionarTarefa(component: HomeComponent): void {
        const dialogRef = this.dialog.open(NovaTarefaComponent, {
            width: '90%',
            maxWidth: '450px',
            disableClose: true,
            autoFocus: true,
            panelClass: 'nova-tarefa-dialog'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                component.isLoading = true;
                
                this.criarUsecase.execute(result).subscribe({
                    next: () => {
                        component.isLoading = false;
                        this.snackBar.open('Tarefa criada com sucesso!', 'Fechar', {
                            duration: 3000,
                            panelClass: 'success-snackbar'
                        });
                        this.listarTarefas(component);
                    },
                    error: (error) => {
                        component.isLoading = false;
                        const errorMessage = error?.error?.message || error?.message || 'Erro ao criar tarefa';
                        this.snackBar.open(errorMessage, 'Fechar', {
                            duration: 3000,
                            panelClass: 'error-snackbar'
                        });
                    }
                });
            }
        });
    }

    editarTarefa(component: HomeComponent, task: TaskResponseDto): void {
        const dialogRef = this.dialog.open(EditarTarefaComponent, {
            width: '90%',
            maxWidth: '450px',
            disableClose: true,
            autoFocus: true,
            panelClass: 'nova-tarefa-dialog',
            data: { task }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                component.isLoading = true;
                
                this.editarUsecase.execute(result).subscribe({
                    next: () => {
                        component.isLoading = false;
                        this.snackBar.open('Tarefa atualizada com sucesso!', 'Fechar', {
                            duration: 3000,
                            panelClass: 'success-snackbar'
                        });
                        this.listarTarefas(component);
                    },
                    error: (error) => {
                        component.isLoading = false;
                        const errorMessage = error?.error?.message || error?.message || 'Erro ao atualizar tarefa';
                        this.snackBar.open(errorMessage, 'Fechar', {
                            duration: 3000,
                            panelClass: 'error-snackbar'
                        });
                    }
                });
            }
        });
    }

    excluirTarefa(component: HomeComponent, task: TaskResponseDto): void {
        const dialogRef = this.dialog.open(ConfirmarExclusaoComponent, {
            width: '90%',
            maxWidth: '420px',
            disableClose: true,
            autoFocus: true,
            data: { task }
        });

        dialogRef.afterClosed().subscribe(confirmed => {
            if (confirmed) {
                component.isLoading = true;
                
                const payload: DeleteTaskRequestDto = {
                    taskId: task.id,
                    taskUserId: task.userId
                };

                this.excluirUsecase.execute(payload).subscribe({
                    next: () => {
                        component.isLoading = false;
                        this.snackBar.open('Tarefa excluída com sucesso!', 'Fechar', {
                            duration: 3000,
                            panelClass: 'success-snackbar'
                        });
                        this.listarTarefas(component);
                    },
                    error: (error) => {
                        component.isLoading = false;
                        const errorMessage = error?.error?.message || error?.message || 'Erro ao excluir tarefa';
                        this.snackBar.open(errorMessage, 'Fechar', {
                            duration: 3000,
                            panelClass: 'error-snackbar'
                        });
                    }
                });
            }
        });
    }
}