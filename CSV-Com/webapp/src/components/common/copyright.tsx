import '../../styles/common/copyright.css';

interface CopyrightProps extends React.HtmlHTMLAttributes<HTMLElement> {
}

export function Copyright(props: CopyrightProps) {
    return (
        <div className="copyright">@ {new Date(). getFullYear()} Copyright: Coaching Bureau Ron de Jong</div>
    );
}