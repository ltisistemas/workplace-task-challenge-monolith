import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './domain/components/home/home.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ListarTarefasUsecaseService } from './domain/usecases/listar-tarefas-usecase.service';
import { CriarTarefaUsecaseService } from './domain/usecases/criar-tarefa-usecase.service';
import { EditarTarefaUsecaseService } from './domain/usecases/editar-tarefa-usecase.service';
import { ExcluirTarefaUsecaseService } from './domain/usecases/excluir-tarefa-usecase.service';
import { TaskRepositoryService } from './infra/repositories/task-repository.service';
import { HomeControllerService } from './domain/controllers/home-controller.service';
import { NovaTarefaComponent } from './domain/components/nova-tarefa/nova-tarefa.component';
import { EditarTarefaComponent } from './domain/components/editar-tarefa/editar-tarefa.component';
import { ConfirmarExclusaoComponent } from './domain/components/confirmar-exclusao/confirmar-exclusao.component';

const routes: Routes = [
    {
        path: '',
        component: HomeComponent
    }
];

@NgModule({
    declarations: [
        HomeComponent,
        NovaTarefaComponent,
        EditarTarefaComponent,
        ConfirmarExclusaoComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild(routes)
    ],
    providers: [
        ListarTarefasUsecaseService,
        CriarTarefaUsecaseService,
        EditarTarefaUsecaseService,
        ExcluirTarefaUsecaseService,
        TaskRepositoryService,
        HomeControllerService
    ]
})
export class HomeModule { }