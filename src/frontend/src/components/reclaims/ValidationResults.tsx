import type { ValidationRuleResult } from "@/types/reclaim";
import styles from "@/styles/reclaims.module.css";

interface ValidationResultsProps {
  results: ValidationRuleResult[];
}

function formatRuleName(name: string): string {
  return name.replace(/([A-Z])/g, " $1").trim();
}

export default function ValidationResults({ results }: ValidationResultsProps) {
  if (results.length === 0) return null;

  return (
    <div className={styles.validationCard}>
      <h3 className={styles.validationTitle}>Validation Results</h3>
      <ul className={styles.ruleList}>
        {results.map((rule) => (
          <li
            key={rule.ruleName}
            className={`${styles.ruleItem} ${rule.passed ? styles.rulePassed : styles.ruleFailed}`}
          >
            <span className={styles.ruleIcon}>
              {rule.passed ? "\u2713" : "\u2717"}
            </span>
            <div className={styles.ruleContent}>
              <span className={styles.ruleName}>
                {formatRuleName(rule.ruleName)}
              </span>
              {rule.failureReason && (
                <span className={styles.ruleReason}>{rule.failureReason}</span>
              )}
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
