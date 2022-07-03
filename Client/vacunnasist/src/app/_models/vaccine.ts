export class Vaccine {
    public id!: string;
    public name!: string;
    canBeRequested!: boolean;
    isActive!: boolean;

    public canApply(age:number, isRisk: boolean) : Boolean {
        // covid: 1, gripe: 3
        if (this.id == '1' || this.name == 'COVID-19') {
            if (age < 18){
                return false;
            }
            return true;
        }
        return true;
    }

    public getMinDate(age:number, isRisk: boolean) : Date {
        let date = new Date();
        // covid: 1, gripe: 3
        if (this.id == '1' || this.name == 'COVID-19') {
            if (age >= 60 || isRisk){
                date.setDate(date.getDate() + 1)
                return date;
            }
            date.setDate(date.getDate() + 7);
            return date;
        }
        if (this.id == '3' || this.name == 'Gripe') {
            if (age >= 60){
                date.setMonth(date.getMonth() + 3);
                return date;
            }
            date.setMonth(date.getMonth() + 6);
            return date;
        }

        //return tomorrow
        date.setDate(date.getDate() + 1)
        return date;
    }

    getMaxDate(age:number, isRisk: boolean) : Date {
        let date = new Date();
        // covid: 1, gripe: 3
        if (this.id == '1' || this.name == 'COVID-19') {
            if (age >= 60 || isRisk){
                date.setDate(date.getDate() + 7)
                return date;
            }
        }

        return new Date('2050-12-31');
    }
}