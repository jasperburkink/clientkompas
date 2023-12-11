import { render, screen } from '@testing-library/react'
import '@testing-library/jest-dom'
import { Button } from '../components/common/button';


test("Button renders successfully", () => {
    render(<Button buttonType={{ type: "Solid" }} text="Button1" className='w-200px h-50px' onClick={() => { alert('Button1'); }} />);

    const element = screen.getByText(/Button/i);

    expect(element).toBeInTheDocument();
})