export class Appointment {
    id!: number;
    patientId!: number;
    vaccineId!: number;
    status!: string;
    notified!: boolean;
    requestedAt!: Date
}