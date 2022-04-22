// Task 1: Swap
{
    var a = 10, b = -5
    a += b
    b = a - b
    a -= b
    alert(a)
    alert(b)
}

// Task 2: Maximum
{
    var a = 10, b = -5
    var c = (a > b) ? a : b
    alert(c)
}

// Task 3: Absolute value
{
    var a = -100
    b = (a < 0) ? -a : a
    alert(b)
}

// Task 4: Formula
{
    var x = -1, z = 3.6
    var y = x*x + 3*x*z + 0.5*z*z - 12/17
    alert(y)
}

// Task 5: Age restriction
{
    var age = Number.parseInt(prompt("Скільки Вам років?"))
    if (age >= 18 && confirm(`Точно ${age}?`)) {
        alert("Доступ відкрито.")
    }
    else {
        alert("ДОСТУП ЗАБОРОНЕНО.")
    }
}

// Task 6: sign
{
    var number = Number(prompt("Enter a number pls..."))
    if (number > 0) {
        alert("POSITIVE NUMBER")
    }
    else if (number < 0) {
        alert("NEGATIVE NUMBER")
    }
    else {
        alert("THAT's ZERO")
    }
}

// Task 7: Password
{
    function ternary (condition, actionT, actionF) {
        if ((typeof ternary == "function")? condition() : condition) {
            actionT()
        }
        else {
            actionF()
        }
    }

    ternary(()=> prompt("Enter a Password") === "zxcvbnm",
            ()=> alert("Good, very good."),
            ()=> alert("Bad, very bad."))
} 

// Task 8: Integral division
{
    function canDivide(a, b) {
        return a % b == 0
    }
    alert(canDivide(9, 3)); // true  
    alert(canDivide(5, 2)); // false 
    alert(canDivide(10000, 5)); // true 
    alert(canDivide(101, 22)); // false 
}

// Task 9: Sum of series
{
    function sumOfSeriesSchool(a, b) {
        return (a + b) * (a + b - 1) / 2
    }

    function sumOfSeriesRecursive(a, b) {
        if (a == b) return b
        else return a + sumOfSeriesRecursive(a + 1, b)
    }

    function sumOfSeriesCycle(a, b) {
        var sum = 0
        for (var i = a; i <= b; i++) {
            sum += i
        }
        return sum
    }

    for (var func of [sumOfSeriesSchool, sumOfSeriesRecursive, sumOfSeriesCycle]) {
        var start = performance.now()
        for (var i=0; i<10000; i++) {
            func(1, 10000)
        }
        var duration = Math.floor(performance.now() - start)
        alert(`${func.name}, 10000 times from 1 to 10000: ${duration} ms`)
    }
}

// Task 10: Fibonacci
{
    function fibonacciCycle(count) {
        if (count == 1) {
            return [1]
        }
        if (count == 2) { 
            return [1, 1]
        }
        else if (count>2) {
            var previous = [1, 1]
            for (var i=2; i<count; i++) 
                previous.push(previous[i-2] + previous[i-1])
            return previous
        }
    }

    function fibonacciRecursive(count) {
        if (count == 1) {
            return [1]
        }
        if (count == 2) { 
            return [1, 1]
        }
        else if (count>2) {
            var previous = fibonacciRecursive(count-1)
            previous.push(previous[count-3] + previous[count-2])
            return previous
        }
    }

    for (var func of [fibonacciCycle, fibonacciRecursive]) {
        alert(`${func.name}: First 10 members: ${func(10)}`)
    }
}

// Task 11
{
    var array = []
    while (true) {
        var item = prompt("Enter something pls...")
        if (item == "exit") break
        else array.push(item)
    }
    for (var item of array) {
        alert(item)
    }
}

// Task 12: Replacement
{
    var items = [3, 5, 0, 9, -1, 0, 0, 12]
    function replace (a, b) {
        items = items.map(item => (item == a) ? b : item)
    }
    replace(0, 99)
    alert(items)
}

// Task 13: Even
{
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 11, 11, 11, 11, 14]
    var evenCount = numbers.reduce ((cnt, item) => (item % 2 == 0) ? cnt+1 : cnt)
    alert(`${evenCount} even numbers in ${JSON.stringify(numbers)}`)
}

// Task 14: Find the maximum
{
    var numbers = [1, 2, 14, -1, 8, 9, -83, 4, 4]
    var max = numbers[0];
    numbers.forEach (item => {
        if (item > max) max = item
    })
    alert(`${max} is maximum in ${JSON.stringify(numbers)}`)
}

// Task 15: Find the average
{
    var numbers = [1, 2, 14, -1, 8, 9, -83, 4, 4]
    var avg = numbers.reduce((prev, item) => prev + item, 0) / numbers.length
    alert(`${avg} is average of ${JSON.stringify(numbers)}`)
}

// Task 16: Negative and positive numbers
{
    var numbers = [3, 6, 0, 9, -1, 0, 0, 12, 0]
    var nPos = numbers.reduce ((cnt, item) => cnt + ((item > 0) ? 1 : 0), 0)
    var nNeg = numbers.reduce ((cnt, item) => cnt + ((item < 0) ? 1 : 0), 0)
    var nNul = numbers.reduce ((cnt, item) => cnt + ((item == 0) ? 1 : 0), 0)
    alert(`${nPos} positive, ${nNeg} negative, ${nNul} zero in ${JSON.stringify(numbers)}`)
} 
