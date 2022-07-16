import { TestGroupSolution } from "./testGroupSolution";

export interface Solution {
    id: number;
    algTaskTag: string;
    algTaskName: string;
    authorUsername: string;
    code: string;
    language: string;
    status: string;
    points: number;
    date: Date;
    memoryLimit: number;
    testGroups: TestGroupSolution[];
}