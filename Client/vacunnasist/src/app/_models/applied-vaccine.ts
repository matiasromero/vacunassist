import { Vaccine } from "./vaccine";

export class AppliedVaccine {
    id!: number;
    userId!: number;
    vaccineId!: number;
    vaccine!:Vaccine;
    appliedDate!: Date;
    appliedBy?: String;
    comment?: string;
}