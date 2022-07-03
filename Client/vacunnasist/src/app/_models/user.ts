import { Office } from 'src/app/_models/office';
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
    age!:number;
    dni!: string;
    belongsToRiskGroup: boolean = false;
    role!: string;
    token!: string;
    isActive!: boolean;
    vaccines: AppliedVaccine[] = [];
    preferedOfficeId?: number;
    preferedOffice?: Office;
    
    public isPatient(): boolean {
         return this.role === 'patient';
    }
}
