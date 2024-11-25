def get_sub_base(sub_bases, sub_minor_radix, sub_major_radix)
  sub = ""
  sub_bases.each do |sub_base|
    s = ""
    parse_result = parse_base(convert_to_base(sub_base, sub_minor_radix, sub_major_radix))
    
    parse_result.each_with_index do |value, i|
      if i.even?
        s += value.to_s
      else
        s += units(value)
      end
    end
    
    if s.to_i <= 6 && s.to_i.to_s == s
      s = units(s.to_i)
    end
    
    sub += s
  end
  sub
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
  
def convert_to_base(dec, radix, sub_radix = -1, bijective = true)

  return "0" if dec == 0

  carry_radix = sub_radix == -1 ? radix : sub_radix
  quotient = dec / radix
  remainder = dec % radix

  if bijective && remainder == 0
    remainder = radix
    quotient -= 1
  end

  if quotient == 0
    remainder.to_s
  elsif quotient > radix
    "#{convert_to_base(quotient, carry_radix)}:#{remainder}"
  else
    "#{quotient}:#{remainder}"
  end
end

def parse_base(s)
  s.split(':').map(&:to_i)
end
