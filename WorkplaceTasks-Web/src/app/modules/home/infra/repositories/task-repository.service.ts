import { Injectable } from '@angular/core';
import { TaskResponseDto, CreateTaskRequestDto, UpdateTaskRequestDto, DeleteTaskRequestDto } from '../dto/task-dto';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class TaskRepositoryService {
    private readonly apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    getTasks(): Observable<TaskResponseDto[]> {
        return this.http.get<TaskResponseDto[]>(`${this.apiUrl}/tasks`);
    }

    createTask(payload: CreateTaskRequestDto): Observable<TaskResponseDto> {
        return this.http.post<TaskResponseDto>(`${this.apiUrl}/tasks`, payload);
    }

    updateTask(payload: UpdateTaskRequestDto): Observable<TaskResponseDto> {
        return this.http.put<TaskResponseDto>(`${this.apiUrl}/tasks/${payload.id}`, payload);
    }

    deleteTask(payload: DeleteTaskRequestDto): Observable<TaskResponseDto> {
        const params = new HttpParams()
            .set('taskId', payload.taskId)
            .set('taskUserId', payload.taskUserId);
        return this.http.delete<TaskResponseDto>(`${this.apiUrl}/tasks`, { params });
    }
}