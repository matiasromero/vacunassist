export class Appointment {
    id!: number;
    patientId!: number;
    patientName!: string;
    patientAge!: number;
    patientRisk!:boolean;
    vaccineId!: number;
    vaccineName?:string;
    status!: string;
    notified!: boolean;
    date?: Date;
    requestedAt!: Date;
    preferedOfficeId?: number;
    preferedOfficeName?: string;
    preferedOfficeAddress?: string;
    vaccinatorId?:number;
    vaccinatorName?:string;
    comment?:string;
    appliedDate?:Date;
}