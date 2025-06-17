// Liste complète des langages de programmation pour le forum


export const programmingLanguages = [
    // Langages populaires
    { value: 'javascript', label: 'JavaScript', icon: '🟨', category: 'web' },
    { value: 'python', label: 'Python', icon: '🐍', category: 'general' },
    { value: 'java', label: 'Java', icon: '☕', category: 'general' },
    { value: 'typescript', label: 'TypeScript', icon: '🔷', category: 'web' },
    { value: 'csharp', label: 'C#', icon: '🟦', category: 'general' },
    { value: 'php', label: 'PHP', icon: '🐘', category: 'web' },
    { value: 'cpp', label: 'C++', icon: '⚡', category: 'systems' },
    { value: 'c', label: 'C', icon: '🔧', category: 'systems' },
    { value: 'go', label: 'Go', icon: '🐹', category: 'systems' },
    { value: 'rust', label: 'Rust', icon: '🦀', category: 'systems' },
    
    // Mobile
    { value: 'swift', label: 'Swift', icon: '🍎', category: 'mobile' },
    { value: 'kotlin', label: 'Kotlin', icon: '🤖', category: 'mobile' },
    { value: 'dart', label: 'Dart', icon: '🎯', category: 'mobile' },
    { value: 'flutter', label: 'Flutter', icon: '💙', category: 'mobile' },
    { value: 'react-native', label: 'React Native', icon: '⚛️', category: 'mobile' },
    
    // Web Frontend
    { value: 'html', label: 'HTML', icon: '🌐', category: 'web' },
    { value: 'css', label: 'CSS', icon: '🎨', category: 'web' },
    { value: 'react', label: 'React', icon: '⚛️', category: 'web' },
    { value: 'vue', label: 'Vue.js', icon: '💚', category: 'web' },
    { value: 'angular', label: 'Angular', icon: '🅰️', category: 'web' },
    { value: 'svelte', label: 'Svelte', icon: '🧡', category: 'web' },
    { value: 'next', label: 'Next.js', icon: '▲', category: 'web' },
    { value: 'nuxt', label: 'Nuxt.js', icon: '💚', category: 'web' },
    
    // Backend & Frameworks
    { value: 'nodejs', label: 'Node.js', icon: '🟢', category: 'backend' },
    { value: 'express', label: 'Express.js', icon: '🚀', category: 'backend' },
    { value: 'django', label: 'Django', icon: '🎸', category: 'backend' },
    { value: 'flask', label: 'Flask', icon: '🌶️', category: 'backend' },
    { value: 'fastapi', label: 'FastAPI', icon: '⚡', category: 'backend' },
    { value: 'spring', label: 'Spring', icon: '🍃', category: 'backend' },
    { value: 'laravel', label: 'Laravel', icon: '🔴', category: 'backend' },
    { value: 'rails', label: 'Ruby on Rails', icon: '💎', category: 'backend' },
    { value: 'aspnet', label: 'ASP.NET', icon: '🟦', category: 'backend' },
    
    // Bases de données
    { value: 'sql', label: 'SQL', icon: '🗄️', category: 'database' },
    { value: 'mysql', label: 'MySQL', icon: '🐬', category: 'database' },
    { value: 'postgresql', label: 'PostgreSQL', icon: '🐘', category: 'database' },
    { value: 'mongodb', label: 'MongoDB', icon: '🍃', category: 'database' },
    { value: 'redis', label: 'Redis', icon: '🔴', category: 'database' },
    { value: 'sqlite', label: 'SQLite', icon: '🪶', category: 'database' },
    { value: 'oracle', label: 'Oracle', icon: '🔶', category: 'database' },
    
    // Langages fonctionnels et autres
    { value: 'ruby', label: 'Ruby', icon: '💎', category: 'general' },
    { value: 'scala', label: 'Scala', icon: '🔺', category: 'general' },
    { value: 'haskell', label: 'Haskell', icon: '🔷', category: 'functional' },
    { value: 'elixir', label: 'Elixir', icon: '💜', category: 'functional' },
    { value: 'erlang', label: 'Erlang', icon: '🔴', category: 'functional' },
    { value: 'clojure', label: 'Clojure', icon: '🟢', category: 'functional' },
    { value: 'fsharp', label: 'F#', icon: '🔷', category: 'functional' },
    
    // Scripts et automation
    { value: 'bash', label: 'Bash', icon: '💻', category: 'scripting' },
    { value: 'powershell', label: 'PowerShell', icon: '💙', category: 'scripting' },
    { value: 'perl', label: 'Perl', icon: '🐪', category: 'scripting' },
    { value: 'lua', label: 'Lua', icon: '🌙', category: 'scripting' },
    
    // Data Science & ML
    { value: 'r', label: 'R', icon: '📊', category: 'data' },
    { value: 'matlab', label: 'MATLAB', icon: '🔢', category: 'data' },
    { value: 'julia', label: 'Julia', icon: '🟣', category: 'data' },
    { value: 'tensorflow', label: 'TensorFlow', icon: '🧠', category: 'data' },
    { value: 'pytorch', label: 'PyTorch', icon: '🔥', category: 'data' },
    
    // Game Development
    { value: 'unity', label: 'Unity', icon: '🎮', category: 'gamedev' },
    { value: 'unreal', label: 'Unreal Engine', icon: '🎯', category: 'gamedev' },
    { value: 'godot', label: 'Godot', icon: '🤖', category: 'gamedev' },
    
    // Cloud & DevOps
    { value: 'docker', label: 'Docker', icon: '🐳', category: 'devops' },
    { value: 'kubernetes', label: 'Kubernetes', icon: '☸️', category: 'devops' },
    { value: 'terraform', label: 'Terraform', icon: '🏗️', category: 'devops' },
    { value: 'ansible', label: 'Ansible', icon: '🔧', category: 'devops' },
    
    // Autres langages
    { value: 'assembly', label: 'Assembly', icon: '⚙️', category: 'systems' },
    { value: 'cobol', label: 'COBOL', icon: '📊', category: 'legacy' },
    { value: 'fortran', label: 'Fortran', icon: '🔢', category: 'scientific' },
    { value: 'pascal', label: 'Pascal', icon: '🔷', category: 'legacy' },
    { value: 'delphi', label: 'Delphi', icon: '🔷', category: 'legacy' },
    { value: 'vb', label: 'Visual Basic', icon: '🟦', category: 'legacy' },
    
    // Configuration et markup
    { value: 'json', label: 'JSON', icon: '📋', category: 'config' },
    { value: 'xml', label: 'XML', icon: '📄', category: 'config' },
    { value: 'yaml', label: 'YAML', icon: '📝', category: 'config' },
    { value: 'markdown', label: 'Markdown', icon: '📖', category: 'markup' },
    { value: 'latex', label: 'LaTeX', icon: '📚', category: 'markup' },
    
    // Divers
    { value: 'other', label: 'Other', icon: '🔀', category: 'other' }
];


export function getLanguageByValue(value) {
    return programmingLanguages.find(lang => lang.value === value);
}

export function getLanguagesByCategory(category) {
    return programmingLanguages.filter(lang => lang.category === category);
}

export function getLanguageIcon(value) {
    const lang = getLanguageByValue(value);
    return lang ? lang.icon : '💻';
}

export function getLanguageLabel(value) {
    const lang = getLanguageByValue(value);
    return lang ? lang.label : value;
}


export const languageCategories = [
    { value: 'all', label: 'All Languages' },
    { value: 'web', label: 'Web Development' },
    { value: 'mobile', label: 'Mobile Development' },
    { value: 'backend', label: 'Backend' },
    { value: 'database', label: 'Database' },
    { value: 'general', label: 'General Purpose' },
    { value: 'systems', label: 'Systems Programming' },
    { value: 'functional', label: 'Functional Programming' },
    { value: 'scripting', label: 'Scripting' },
    { value: 'data', label: 'Data Science & ML' },
    { value: 'gamedev', label: 'Game Development' },
    { value: 'devops', label: 'DevOps & Cloud' },
    { value: 'scientific', label: 'Scientific Computing' },
    { value: 'config', label: 'Configuration' },
    { value: 'markup', label: 'Markup Languages' },
    { value: 'legacy', label: 'Legacy Languages' },
    { value: 'other', label: 'Other' }
]; 