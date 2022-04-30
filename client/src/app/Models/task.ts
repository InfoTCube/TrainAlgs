import { TestGroup } from "./testGroup";

export interface Task {
    name: string;
    nameTag: string;
    content: string;
    authorUsername: string;
    memoryLimit: number;
    testGroups: TestGroup[];
}