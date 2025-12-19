import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CreateTaskRequestDto } from '../../../infra/dto/task-dto';

@Component({
    selector: 'app-nova-tarefa',
    templateUrl: './nova-tarefa.component.html',
    styleUrls: ['./nova-tarefa.component.scss']
})
export class NovaTarefaComponent {
    form: FormGroup;
    isLoading = false;

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<NovaTarefaComponent>
    ) {
        this.form = this.fb.group({
            title: ['', [Validators.required, Validators.minLength(3)]],
            description: ['', [Validators.required, Validators.minLength(5)]]
        });
    }

    onSubmit(): void {
        if (this.form.valid && !this.isLoading) {
            this.isLoading = true;
            const payload: CreateTaskRequestDto = {
                title: this.form.value.title,
                description: this.form.value.description
            };
            // Pequeno delay para mostrar o loading antes de fechar
            setTimeout(() => {
                this.dialogRef.close(payload);
            }, 100);
        }
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}
