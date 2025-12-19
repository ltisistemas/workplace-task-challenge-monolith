import { Injectable } from '@angular/core';
import { TaskResponseDto, DeleteTaskRequestDto } from '../../infra/dto/task-dto';
import { Observable } from 'rxjs';
import { TaskRepositoryService } from '../../infra/repositories/task-repository.service';

export interface ExcluirTarefaUsecase {
    execute(payload: DeleteTaskRequestDto): Observable<TaskResponseDto>;
}

@Injectable({
    providedIn: 'root'
})
export class ExcluirTarefaUsecaseService implements ExcluirTarefaUsecase {
    constructor(private repository: TaskRepositoryService) { }

    execute(payload: DeleteTaskRequestDto): Observable<TaskResponseDto> {
        return this.repository.deleteTask(payload);
    }
}

