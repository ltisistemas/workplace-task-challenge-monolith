import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskResponseDto } from '../../../infra/dto/task-dto';

@Component({
    selector: 'app-confirmar-exclusao',
    templateUrl: './confirmar-exclusao.component.html',
    styleUrls: ['./confirmar-exclusao.component.scss']
})
export class ConfirmarExclusaoComponent {
    task: TaskResponseDto;
    isLoading = false;

    constructor(
        private dialogRef: MatDialogRef<ConfirmarExclusaoComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { task: TaskResponseDto }
    ) {
        this.task = data.task;
    }

    confirm(): void {
        this.isLoading = true;
        setTimeout(() => {
            this.dialogRef.close(true);
        }, 100);
    }

    cancel(): void {
        this.dialogRef.close(false);
    }
}

