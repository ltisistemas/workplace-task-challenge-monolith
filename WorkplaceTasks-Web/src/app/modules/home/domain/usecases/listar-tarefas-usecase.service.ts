import { Injectable } from '@angular/core';
import { TaskResponseDto } from '../../infra/dto/task-dto';
import { Observable } from 'rxjs';
import { TaskRepositoryService } from '../../infra/repositories/task-repository.service';

export interface ListarTarefasUsecase {
    execute(): Observable<TaskResponseDto[]>;
}

@Injectable({
    providedIn: 'root'
})
export class ListarTarefasUsecaseService implements ListarTarefasUsecase {
    constructor(private repository: TaskRepositoryService) { }

    execute(): Observable<TaskResponseDto[]> {
        return this.repository.getTasks();
    }
}