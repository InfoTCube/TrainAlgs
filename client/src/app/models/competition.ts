export interface Competition {
    id: number;
    name: string;
    description: string;
    tasks: Task[];
    submissionsLimit: string;
    startDate: Date;
    endDate: Date;
    organizerUsername: string;
}