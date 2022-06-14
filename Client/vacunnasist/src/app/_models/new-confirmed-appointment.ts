export class NewConfirmedAppointmentRequest {
    vaccineId!: number;
    vaccinatorId!: number;
    officeId!: number;
    patientId!: number;
    date!: string;
    currentId?:number;
}