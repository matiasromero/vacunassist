export class Appointment {
    id!: number;
    patientId!: number;
    patientName!: string;
    vaccineId!: number;
    vaccineName?:string;
    status!: string;
    notified!: boolean;
    requestedAt!: Date;
    preferedOfficeId?: number;
    preferedOfficeName?: string;
    preferedOfficeAddress?: string;
    vaccinatorId?:string;
    vaccinatorName?:string;
    comment?:string;
    appliedDate?:Date;
}