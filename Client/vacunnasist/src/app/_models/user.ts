import { AppliedVaccine } from "./applied-vaccine";

export class User {
    id!: string;
    userName!: string;
    address!: string;
    password!: string;
    fullName!: string;
    gender!: string;
    phoneNumber!: string;
    email!: string;
    birthDate!: Date;
    dni!: string;
    belongsToRiskGroup: boolean = false;
    role!: string;
    token!: string;
    isActive!: boolean;
    vaccines: AppliedVaccine[] = [];
    preferedOfficeId?: number;
    
    public isPatient(): boolean {
         return this.role === 'patient';
    }
}