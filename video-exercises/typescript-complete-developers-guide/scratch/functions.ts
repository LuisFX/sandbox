const add = ( a: number, b:number ) : number => {
    return a + b
}

function substract(a: number, b: number): number {
    return a - b
}

function divide( a: number, b: number ) : number {
    return a / b
}

const multiply = function (a: number, b: number) : number {
    return a * b
}

class Test {
    constructor(private readonly start: Date) {}

}

export default Test


export default class Gigasecond {
    constructor(private readonly start: Date) {}
  
    date(): Date {
      return new Date(this.start.getTime() + 10 ** 12);
    }
}