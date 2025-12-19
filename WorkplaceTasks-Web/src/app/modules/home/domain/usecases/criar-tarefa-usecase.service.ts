import { Injectable } from '@angular/core';
import { TaskResponseDto, CreateTaskRequestDto } from '../../infra/dto/task-dto';
import { Observable } from 'rxjs';
import { TaskRepositoryService } from '../../infra/repositories/task-repository.service';

export interface CriarTarefaUsecase {
    execute(payload: CreateTaskRequestDto): Observable<TaskResponseDto>;
}

@Injectable({
    providedIn: 'root'
})
export class CriarTarefaUsecaseService implements CriarTarefaUsecase {
    constructor(private repository: TaskRepositoryService) { }

    execute(payload: CreateTaskRequestDto): Observable<TaskResponseDto> {
        return this.repository.createTask(payload);
    }
}

