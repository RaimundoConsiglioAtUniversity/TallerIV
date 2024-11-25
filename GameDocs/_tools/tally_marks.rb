def convert_to_tally(num, base = 5)

    s = ""
    quotient = num / base
    remainder = num % base

    quotient.times do
        s += units(base)
    end

    if remainder != 0
        s += units(remainder)
    end

    return s
end

def units(number)
    case number
    when 0
      "0"
    when 1
      "A"
    when 2
      "B"
    when 3
      "C"
    when 4
      "D"
    when 5
      "E"
    when 6
      "F"
    else
      ""
    end
  end
