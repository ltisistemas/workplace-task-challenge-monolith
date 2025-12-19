import { Injectable } from '@angular/core';
import { TaskResponseDto, UpdateTaskRequestDto } from '../../infra/dto/task-dto';
import { Observable } from 'rxjs';
import { TaskRepositoryService } from '../../infra/repositories/task-repository.service';

export interface EditarTarefaUsecase {
    execute(payload: UpdateTaskRequestDto): Observable<TaskResponseDto>;
}

@Injectable({
    providedIn: 'root'
})
export class EditarTarefaUsecaseService implements EditarTarefaUsecase {
    constructor(private repository: TaskRepositoryService) { }

    execute(payload: UpdateTaskRequestDto): Observable<TaskResponseDto> {
        return this.repository.updateTask(payload);
    }
}

