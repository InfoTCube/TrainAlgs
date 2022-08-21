export interface Member {
    username: string;
    created: Date;
    lastActive: Date;
    country: string;
    description: string;
    isModerator: boolean;
    graphChart: GraphChart;
}

export interface GraphChart {
  solutions: object[];
  solvedAllTime: number;
  solvedLastYear: number;
  solvedLastMonth: number;
  solvedInRowAllTime: number;
  solvedInRowLastYear: number;
  solvedInRowLastMonth: number;
}
