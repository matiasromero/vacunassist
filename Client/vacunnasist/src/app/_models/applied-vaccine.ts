import { Vaccine } from "./vaccine";

export class AppliedVaccine {
    userId!: number;
    vaccineId!: number;
    vaccine!:Vaccine;
    appliedDate!: Date;
}