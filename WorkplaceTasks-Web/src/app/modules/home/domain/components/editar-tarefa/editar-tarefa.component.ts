import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskResponseDto, UpdateTaskRequestDto, TaskStatus } from '../../../infra/dto/task-dto';

@Component({
    selector: 'app-editar-tarefa',
    templateUrl: './editar-tarefa.component.html',
    styleUrls: ['./editar-tarefa.component.scss']
})
export class EditarTarefaComponent implements OnInit {
    form: FormGroup;
    task: TaskResponseDto;
    isLoading = false;

    statusOptions = [
        { value: TaskStatus.Pending, label: 'Pendente' },
        { value: TaskStatus.InProgress, label: 'Em Progresso' },
        { value: TaskStatus.Done, label: 'Concluída' }
    ];

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<EditarTarefaComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { task: TaskResponseDto }
    ) {
        this.task = data.task;
        this.form = this.fb.group({
            title: ['', [Validators.required, Validators.minLength(3)]],
            description: ['', [Validators.required, Validators.minLength(5)]],
            status: [TaskStatus.Pending, [Validators.required]]
        });
    }

    ngOnInit(): void {
        this.form.patchValue({
            title: this.task.title,
            description: this.task.description,
            status: this.task.status
        });
    }

    onSubmit(): void {
        if (this.form.valid && !this.isLoading) {
            this.isLoading = true;
            const payload: UpdateTaskRequestDto = {
                id: this.task.id,
                title: this.form.value.title,
                description: this.form.value.description,
                status: this.form.value.status,
                taskUserId: this.task.userId
            };
            setTimeout(() => {
                this.dialogRef.close(payload);
            }, 100);
        }
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}

