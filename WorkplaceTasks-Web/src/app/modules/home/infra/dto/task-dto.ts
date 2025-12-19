export enum TaskStatus {
  Pending = 0,
  InProgress = 1,
  Done = 2,
  Deleted = 3
}

// {
//   "id": "bcfa0708-dcf2-42cd-bf88-5a556068504b",
//   "userId": "b89dd545-cb5a-482b-9035-5da17d445d62",
//   "title": "Atividade 01",
//   "description": "Descrição detalhada",
//   "status": 0,
//   "createdAt": "2025-12-19T00:38:25.820422Z",
//   "updatedAt": "2025-12-19T00:38:25.820422Z"
// }
export interface TaskResponseDto {
  id: string;
  userId: string;
  title: string;
  description: string;
  status: TaskStatus;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTaskRequestDto {
  title: string;
  description: string;
}

export interface UpdateTaskRequestDto {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  taskUserId: string;
}

export interface DeleteTaskRequestDto {
  taskId: string;
  taskUserId: string;
}
